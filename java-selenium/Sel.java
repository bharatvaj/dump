import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.firefox.FirefoxDriver;

public class Sel {
    static WebDriver driver;

    public static void main(String[] args) {

        System.setProperty("webdriver.gecko.driver", "/Users/bharat-9052/.cache/downloads/geckodriver"); //
        // Setting system properties of
        // FirefoxDriver
        WebDriver driver = new FirefoxDriver(); // Creating an object of FirefoxDriver
        driver.manage().window().maximize();
        driver.manage().deleteAllCookies();
        // driver.manage().timeouts().pageLoadTimeout(40, TimeUnit.SECONDS);
        // driver.manage().timeouts().implicitlyWait(30, TimeUnit.SECONDS);
        driver.get("https://www.google.com/");
        driver.findElement(By.name("q")).sendKeys("Browserstack Guide"); // name locator for text box
        WebElement searchbutton = driver.findElement(By.name("btnK"));// name locator for google search
        searchbutton.click();
        driver.quit();
    }
}
