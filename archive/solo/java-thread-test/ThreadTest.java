import java.lang.Thread;
import java.lang.InterruptedException;

class ThreadTest {


    public ThreadTest() throws InterruptedException {
        Thread t = new Thread(null, () -> {
            try {
                Thread.sleep(1000);
                Thread.interrupted();
            } catch (InterruptedException ie) {
                System.out.println("Catched Interrupt");
            }
            System.out.println("Hello");
        });
        t.start();
        t.interrupt();
    }

    public static void main(String[] args) {
        try {
            new ThreadTest();
        } catch (InterruptedException ie) {
            System.out.println("Interrupted Exception" + ie.getMessage());
        }
    }
}