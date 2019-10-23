package polymtl.equipe3.clientlegerv2.model

class Drawing() {

    val segments: MutableList<Segment> = mutableListOf()
    var width: Int = 0
    var height: Int = 0

    fun toSVG(): String {
        val svg = StringBuilder()
        svg.append("<svg height=\"$height\" width=\"$width\">")
        for (segment in segments) {
            svg.append(segment.toSVG())
        }
        svg.append("</svg>")
        return svg.toString()
    }
}