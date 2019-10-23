package polymtl.equipe3.clientlegerv2.ui.draw

import android.content.Context
import android.graphics.*
import android.util.AttributeSet
import android.view.View
import android.graphics.Paint.DITHER_FLAG
import android.graphics.Paint.Cap
import android.graphics.Paint.Join
import android.util.Log
import android.view.MotionEvent
import polymtl.equipe3.clientlegerv2.model.Drawing
import polymtl.equipe3.clientlegerv2.model.Segment
import kotlin.math.abs
import androidx.lifecycle.LifecycleOwner
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.Observer
import polymtl.equipe3.clientlegerv2.model.DrawModeEnum


class DrawView @JvmOverloads constructor(
    context: Context, attrs: AttributeSet? = null, defStyleAttr: Int = 0
) : View(context, attrs, defStyleAttr) {

    companion object {
        const val BRUSH_SIZE = 20F
        const val DEFAULT_COLOR = Color.BLACK
        const val DEFAULT_BG_COLOR = Color.WHITE
    }

    private var currentColor: Int = DEFAULT_COLOR
    private var currentStrokeWidth: Float = BRUSH_SIZE
    private var currentMode: DrawModeEnum = DrawModeEnum.PENCIL
    private var currentCap: Cap = Cap.SQUARE

    private var viewWidth: Int = 0
    private var viewHeight: Int = 0
    private var fingerPosX: Float = 0.0F
    private var fingerPosY: Float = 0.0F

    private var mPaint: Paint = Paint()
    private var mBitmap: Bitmap? = null
    private var mCanvas: Canvas? = null
    private var rect: Rect? = null
    private val mBitmapPaint = Paint(DITHER_FLAG)

    private var drawing: Drawing? = null
    private var currentSegment: Segment? = null


    init {
        mPaint.isAntiAlias = true
        mPaint.isDither = true
        mPaint.color = DEFAULT_COLOR
        mPaint.style = Paint.Style.STROKE
        mPaint.strokeJoin = Join.ROUND
        mPaint.xfermode = null
        mPaint.alpha = 0xff
    }

    /**
     * Initializes the drawView, giving it all the liveData to observe to update its values.
     */
    fun initialize(lifecycleOwner: LifecycleOwner,
                   drawing: Drawing,
                   mutableStroke: MutableLiveData<Float>,
                   mutableColor: MutableLiveData<Int>,
                   mutableMode: MutableLiveData<DrawModeEnum>) {

        this.drawing = drawing
        this.drawing!!.width = viewWidth
        this.drawing!!.height = viewHeight
        mutableStroke.observe(lifecycleOwner, Observer {
            currentStrokeWidth = it
        })
        mutableColor.observe(lifecycleOwner, Observer {
            currentColor = it
        })
        mutableMode.observe(lifecycleOwner, Observer {
            setMode(it)
        })
    }

    /**
     * Clears the view, deleting all segments in the current drawing
     */
    fun clearAll() {
        drawing?.let{
            it.segments.clear()
        }
        invalidate()
    }

    /**
     * Sets the current mode of the DrawView.
     */
    private fun setMode(mode: DrawModeEnum) {
        when(mode) {
            DrawModeEnum.PENCIL -> {
                currentCap = Cap.SQUARE
            }
            DrawModeEnum.BRUSH -> {
                currentCap = Cap.ROUND
            }
        }
        currentMode = mode
    }

    /**
     * Gets called every time the view size changes. This is used because on creation, the view does
     * not have a size yet, it's only set when the parent fragments draw the view. But since we need
     * to know the width and height of this view so we can create a drawing of the according size,
     * we need to subscribe to this sizeChanged event.
     */
    override fun onSizeChanged(xNew: Int, yNew: Int, xOld: Int, yOld: Int) {
        super.onSizeChanged(xNew, yNew, xOld, yOld)

        viewWidth = xNew
        viewHeight = yNew
        drawing!!.width = viewWidth
        drawing!!.height = viewHeight
    }

    /**
     * This is the call that renders the view. This is where we draw all the drawing segments.
     */
    override fun onDraw(canvas: Canvas?) {
        drawing?.let {
            canvas?.save()
            if (mBitmap == null) {
                mBitmap = Bitmap.createBitmap(width, height, Bitmap.Config.ARGB_8888)
                mCanvas = Canvas(mBitmap!!)
                rect = Rect(0,0,viewWidth, viewHeight)
            }

            mCanvas!!.drawColor(DEFAULT_BG_COLOR)

            for (segment in it.segments) {
                mPaint.color = segment.color
                mPaint.strokeWidth = segment.strokeWidth.toFloat()
                mPaint.strokeCap = segment.strokeCap
                mCanvas!!.drawPath(segment.path!!, mPaint)

            }

            canvas!!.drawBitmap(mBitmap!!, null, rect!!, mBitmapPaint)
            canvas.restore()
        }
    }

    /**
     * This events gets called when interacts with the touch screen
     */
    override fun onTouchEvent(event: MotionEvent?): Boolean {

        when(event!!.action) {
            // When the user starts touching the screen
            MotionEvent.ACTION_DOWN -> {
                when(currentMode) {
                    DrawModeEnum.PENCIL -> onDrawStart(event.x, event.y)
                    DrawModeEnum.BRUSH -> onDrawStart(event.x, event.y)
                    DrawModeEnum.SEGMENT_ERASER -> onEraseStart(event.x, event.y)
                    DrawModeEnum.ERASER -> onEraseStart(event.x, event.y)
                }
                invalidate()
            }
            // When the user moves his finger on the screen
            MotionEvent.ACTION_MOVE -> {
                when(currentMode) {
                    DrawModeEnum.PENCIL -> onDrawMove(event.x, event.y)
                    DrawModeEnum.BRUSH -> onDrawMove(event.x, event.y)
                    DrawModeEnum.SEGMENT_ERASER -> onEraseMove(event.x, event.y)
                    DrawModeEnum.ERASER -> onEraseMove(event.x, event.y)
                }
                invalidate()
            }
            // When the user lift his finger up from the screen/ when he stops touching it
            MotionEvent.ACTION_UP -> {
                when(currentMode) {
                    DrawModeEnum.PENCIL -> onDrawEnd(event.x, event.y)
                    DrawModeEnum.BRUSH -> onDrawEnd(event.x, event.y)
                    DrawModeEnum.SEGMENT_ERASER -> onEraseEnd(event.x, event.y)
                    DrawModeEnum.ERASER -> onEraseEnd(event.x, event.y)
                }
                invalidate()
            }
        }
        return true
    }

    private fun onDrawStart(x: Float, y: Float) {
        fingerPosX = x
        fingerPosY = y

        currentSegment = Segment(currentStrokeWidth.toInt(), currentColor, currentCap)
        currentSegment!!.points.add(Point(x.toInt(), y.toInt()))
        currentSegment!!.savePath()
        drawing!!.segments.add(currentSegment!!)
    }

    private fun onDrawMove(x: Float, y: Float) {
        val dx = abs(fingerPosX - x)
        val dy = abs(fingerPosY - y)
        if (dx > 5 || dy > 5) {
            fingerPosX = x
            fingerPosY = y
            currentSegment!!.savePath()
            currentSegment!!.points.add(Point(x.toInt(), y.toInt()))
        }
    }

    private fun onDrawEnd(x: Float, y: Float) {
        currentSegment!!.points.add(Point(x.toInt(), y.toInt()))
        Log.d("TEST_SVG", drawing!!.toSVG())
    }

    private fun onEraseStart(x: Float, y: Float) {
        eraseCollidingPaths(x.toInt(), y.toInt())
    }

    private fun onEraseMove(x: Float, y: Float) {
        eraseCollidingPaths(x.toInt(), y.toInt())
    }

    private fun onEraseEnd(x: Float, y: Float) {
        eraseCollidingPaths(x.toInt(), y.toInt())
        Log.d("TEST_SVG", drawing!!.toSVG())
    }

    /**
     * Checks if the point given is colliding with a path, and if so, either deletes the path or
     * delete the point and create 2 sub-segments before and after the colliding point, depending
     * on the erasing mode.
     */
    private fun eraseCollidingPaths(x: Int, y: Int) {
        // Temp list of segment to delete, and to create in our drawing
        // The segments to create are the "sub-segments" of the newly splitted segment
        var toDelete: MutableList<Segment>
        var toCreate: MutableList<Segment>
        val padding = 20
        val touchRect = Rect(x - padding, y - padding, x + padding,y - padding)

        do {
            toDelete = mutableListOf()
            toCreate = mutableListOf()
            eraseForAllSegment(drawing!!.segments, touchRect, toDelete, toCreate)
            drawing!!.segments.addAll(toCreate)
            drawing!!.segments.removeAll(toDelete)
        } while (toCreate.size != 0)
    }

    private fun eraseForAllSegment(segments: MutableList<Segment>, touchRect: Rect, toDelete: MutableList<Segment>, toCreate: MutableList<Segment>){
        for (segment in segments) {
            // For each point in our segment, we wil check if the rectangle between this point and the next one intersect
            for ((index, point) in segment.points.withIndex()) {
                var nextPoint = segment.points[index]
                // the nextPoint is the same as the current point if we're currently at the last point of our points list
                if (index < segment.points.size-1) {
                    nextPoint = segment.points[index + 1]
                }
                // We get the rectangle between this point and the next one
                val collisionRect = getCollisionRect(point, nextPoint, segment.strokeWidth / 2)

                // Now we do the actual check, to see if our finger is touching a part of a segment
                if (collisionRect.intersect(touchRect)) {
                    when(currentMode) {
                        DrawModeEnum.SEGMENT_ERASER -> {
                            // In this mode we simply delete the segment
                            toDelete.add(segment)
                        }
                        DrawModeEnum.ERASER -> {
                            // In this mode we delete the segment, an create 2 sub-segment, if applicable
                            toDelete.add(segment)
                            // We only create the first sub-segment if we have more than one point
                            // and if we're not deleting the first point of the segment
                            // This is just so we don't end up with an empty segment
                            if (segment.points.size > 1 && index > 0) {
                                val segment1 = Segment(segment.strokeWidth, segment.color, segment.strokeCap)
                                segment1.points.addAll(segment.points.subList(0, index))
                                if (segment1.points.size > 1) {
                                    segment1.savePath()
                                    toCreate.add(segment1)
                                }
                            }
                            // We only create the second sub-segment if we're at the last point of the segment
                            // This is just so we don't end up with an empty segment
                            if (index < segment.points.size - 1) {
                                val segment2 = Segment(segment.strokeWidth, segment.color, segment.strokeCap)
                                segment2.points.addAll(segment.points.subList(index + 1, segment.points.size))
                                if (segment2.points.size > 1) {
                                    segment2.savePath()
                                    toCreate.add(segment2)
                                }
                            }
                        }
                    }
                    break
                }
            }
        }
    }

    /**
     * Gets the RectF for the two given points, with the correct left, top, right and bottom values,
     * depending on the direction of the vector between the 2 points
     * @param point1 the origin point, the origin of the vector
     * @param point2 the destination point, the destination of the vector
     * @param padding the padding of the rectangle, so the rectangle is the same width as the stroke. Should always be currentStrokeWidth / 2
     * @return The rectangle to use to check for collision
     */
    private fun getCollisionRect(point1: Point, point2: Point, padding: Int): Rect {
        val dx = point2.x - point1.x
        val dy = point2.y - point1.y
        var rect = Rect()
        //
        // ↖
        if (dx < 0 && dy < 0) {
            rect = Rect(
                point2.x - padding,
                point2.y -  + padding,
                point1.x + padding,
                point1.y + padding
            )
        }
        // ↙
        if (dx < 0 && dy >= 0) {
            rect = Rect(
                point2.x - padding,
                point1.y - padding,
                point1.x + padding,
                point2.y + padding
            )
        }
        // ↗
        if (dx >= 0 && dy < 0) {
            rect = Rect(
                point1.x - padding,
                point2.y - padding,
                point2.x + padding,
                point1.y + padding
            )
        }
        // ↘
        if (dx > 0 && dy > 0) {
            rect = Rect(
                point1.x - padding,
                point1.y - padding,
                point2.x + padding,
                point2.y + padding
            )
        }
        //When both points are exactly at the same pixels:
        if (dx == 0 && dy == 0) {
            rect = Rect(
                point1.x - padding,
                point1.y - padding,
                point2.x + padding,
                point2.y + padding
            )
        }
        return rect
    }
}