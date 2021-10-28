import java.io.*;

class NumberExpand {
  static String expand(char c, int n){
    StringBuffer buffer = new StringBuffer();
    for(int i = 0; i < n; i++){
      buffer.append(c);
    }
    return buffer.toString();
  }
}
