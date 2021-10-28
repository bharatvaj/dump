import java.io.*;
import java.util.*;

public class Expand {

static String expand(char c, int n){
        StringBuilder buffer = new StringBuilder();
        for(int i = 0; i < n; i++)
                buffer.append(c);
        return buffer.toString();
}

public static void main(String args[]){

        Scanner sc = new Scanner(System.in);
        String in = sc.next();
        //input processing
        for(int i = 0; i < in.length(); i++) {
                char c = in.charAt(i);
                StringBuilder sb = new StringBuilder();
                sb.append(in.charAt(++i));
                if((i + 1) < in.length() && Character.isDigit(in.charAt(i+1)))
                        sb.append(in.charAt(++i));
                int n = Integer.parseInt(sb.toString());
                //output
                System.out.print(expand(c, n));
        }
}
}
