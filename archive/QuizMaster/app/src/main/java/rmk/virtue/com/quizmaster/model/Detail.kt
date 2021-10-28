package rmk.virtue.com.quizmaster.model

import java.util.ArrayList

data class Detail(
        var type: String,
        var content: List<String>) {
    constructor() : this("", ArrayList())
}
