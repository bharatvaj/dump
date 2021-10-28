package view;

import java.awt.Color;
import java.awt.Font;
import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

import javax.swing.JFrame;
import javax.swing.JTextPane;
import javax.swing.event.DocumentEvent;
import javax.swing.event.DocumentListener;
import javax.swing.text.AttributeSet;
import javax.swing.text.BadLocationException;
import javax.swing.text.DefaultStyledDocument;
import javax.swing.text.Style;
import javax.swing.text.StyleConstants;
import javax.swing.text.StyleContext;
import javax.swing.text.StyledDocument;

import helperClass.Expression;

public class AJTextField extends JTextPane {

	/*
	 * AJTextField - Arithmetic JTextField
	 * Highlights Operators and functions(mostly)
	 */
	private static final Color OPERATOR_COLOR = Color.RED;
	private static final Color FUNCTION_COLOR = Color.BLUE;
	private static final Color BRACKET_COLOR = Color.YELLOW;
	private static final String[] OPERATORS = {"+","-","*","/"};
	private static final String[] BRACKETS = {"(",")","{","}"};
	
	//has the running rootFrame instance
	JFrame rootFrame = null;
	
	
	private MJTextPane resultView = null;
	private Style style;
	private StyledDocument doc;

    Expression expression = new Expression("0");
	
	public AJTextField()
	{
		getDocument().addDocumentListener(new SimpleDocumentListener());
	}
	public void appendText(String str)
	{
		styleWord(str);
        try { 
        	doc.insertString(doc.getLength(), str,style); 
        }
        catch (BadLocationException e){
        }
	}
	
	
	private void styleWord(String str)
	{
		doc = getStyledDocument();

		Color col = colorWord(str);
        style = addStyle("ColorStyle", null);
        StyleConstants.setForeground(style, col);
        StyleConstants.setFontSize(style, 30);
	}
	
	public void evaluate()
	{
		try {
    	expression = new Expression(getText());
        BigDecimal bd = expression.eval();
        resultView.setText(String.valueOf(bd));
		}
		catch(Exception ex)
		{
			ex.printStackTrace();
		}
	}
	
		
	public final Color colorWord(String str){
		Color col = getForeground();
		for (String i : OPERATORS)
		{
			if(i.equals(str))
			{
				col = Color.RED;
			}
		}
		for(String i :BRACKETS)
		{
			if(i.equals(str))
			{
				col = Color.YELLOW;
			}
		}
		return col;
	}
	
	class SimpleDocumentListener implements DocumentListener
	{

		@Override
		public void insertUpdate(DocumentEvent e) {
		}

		@Override
		public void removeUpdate(DocumentEvent e) {
			
		}

		@Override
		public void changedUpdate(DocumentEvent e) {
			try {
				 evaluate();
				System.out.print(doc.getText(doc.getLength(),0));
			styleWord(doc.getText(doc.getLength(),1));
			//e.getDocument().insertString(e.getDocument().getLength(), e.getDocument().getText(e.getDocument().getLength(),0), style);
		} catch (BadLocationException e1) {
			e1.printStackTrace();
		}
		}
		
	}

	public void setResultTarget(MJTextPane resultView, JFrame frame) {
		this.resultView = resultView;
		this.rootFrame = frame;
	}
}


