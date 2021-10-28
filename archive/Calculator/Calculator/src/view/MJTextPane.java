package view;

import java.awt.Color;

import javax.swing.JTextPane;
import javax.swing.text.Style;
import javax.swing.text.StyleConstants;
import javax.swing.text.StyledDocument;

public class MJTextPane extends JTextPane {
	/* MJTextPane - Mathematical JTextPane
	 * The resultant text is always in setting provided ny user
	 */

	private Style style;
	private StyledDocument doc;
	
	
	public MJTextPane()
	{
	}
	

	private void styleWord(String str)
	{
		doc = getStyledDocument();

		Color col = colorWord(str);
        style = addStyle("ColorStyle", null);
        StyleConstants.setForeground(style, col);
        StyleConstants.setFontSize(style, 30);
	}


	private Color colorWord(String str) {
		Color col = getForeground();
		return col;
	}
}
