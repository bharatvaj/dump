package rmk.virtue.com.quizmaster.model;

public class Question {
    private String answer;
    private String choice1;
    private String choice2;
    private String choice3;
    private String choice4;
    private String question;

    public Question() {}

    public Question(String answer, String choice1, String choice2, String choice3, String choice4, String question) {
        this.answer = answer;
        this.choice1 = choice1;
        this.choice2 = choice2;
        this.choice3 = choice3;
        this.choice4 = choice4;
        this.question = question;
    }

    public String getAnswer() {
        return answer;
    }

    public String getChoice1() {
        return choice1;
    }

    public String getChoice2() {
        return choice2;
    }

    public String getChoice3() {
        return choice3;
    }

    public String getChoice4() {
        return choice4;
    }

    public String getQuestion() {
        return question;
    }

    public void setAnswer(String answer) {
        this.answer = answer;
    }

    public void setChoice1(String choice1) {
        this.choice1 = choice1;
    }

    public void setChoice2(String choice2) {
        this.choice2 = choice2;
    }

    public void setChoice3(String choice3) {
        this.choice3 = choice3;
    }

    public void setChoice4(String choice4) {
        this.choice4 = choice4;
    }

    public void setQuestion(String question) {
        this.question = question;
    }

}