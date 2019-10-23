package polymtl.equipe3.clientlegerv2.model

import android.graphics.Paint
import android.graphics.Path
import android.graphics.Point

class Segment(val strokeWidth: Int, val color: Int, val strokeCap: Paint.Cap ) {

    val points: MutableList<Point> = mutableListOf()
    var path: Path? = null

    fun toSVG(): String {
        val svg = StringBuilder()
        svg.append("<polyline points=\"")
        for (point in points){
            svg.append("${point.x}, ${point.y} ")
        }
        svg.append("\" style=\"fill:none;stroke:${String.format("#%06X", 0xFFFFFF and color)};stroke-width:$strokeWidth\" />")
        return svg.toString()
    }

    fun savePath() {
        path = Path()

        if (points.size == 0) {
            return
        }

        path!!.moveTo(points[0].x.toFloat(), points[0].y.toFloat())

        for (point in points) {
            path!!.lineTo(point.x.toFloat(), point.y.toFloat())
        }
    }
}