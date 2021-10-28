package rmk.virtue.com.quizmaster.model

data class Link(
        var icon: String,
        var url: String) {
    constructor() : this("", "")
}
