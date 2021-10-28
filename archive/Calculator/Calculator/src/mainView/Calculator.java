package mainView;

/*Program to evaluate simple arithmetic functions*/
import java.awt.BorderLayout;
import java.awt.Color;
import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.GridLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.math.BigDecimal;

import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JTextField;
import javax.swing.UIManager;
import javax.swing.UnsupportedLookAndFeelException;

import view.AJTextField;
import view.MJTextPane;

import javax.swing.JLabel;
import javax.swing.JPanel;
//import java.awt.AbsoluteLayout;

class Calculator extends JFrame 
/*the class extends JFrame 
so there is no need to 
seperately initialize JFrame*/
{
	
	JPanel rootPanel = null; //this panel contains all the components
	AJTextField inputView = null; //this custom control allows us to format the text input without any effort
	MJTextPane resultView = null;//this custom textPane displays the answer in specified notation
	JPanel keyPanel = null;
    Calculator() //overriding default constructor
    {
        initComponents(); //initialize the components
        keyPanel.add(addArithmeticPanel(),BorderLayout.EAST);
    }
    
    private JPanel addArithmeticPanel() {
    	JPanel panel = new JPanel(new GridBagLayout());
    	for(int i=0;i<4;i++)
    	{
    		GridBagConstraints gbc = new GridBagConstraints();
    		gbc.gridx = 0;
        	gbc.gridy = i;
        	
        	gbc.weightx = 0.5;
        	gbc.weighty = 0.5;
        	gbc.fill = GridBagConstraints.BOTH;
    		final JButton btn = new JButton();
    		btn.setPreferredSize(new Dimension(50,50));
    		switch(i)
    		{
    		case 0:
    			btn.setText("+");
    			break;
    		case 1:
    			btn.setText("-");
    			break;
    		case 2:
    			btn.setText("*");
    			break;
    		case 3:
    			btn.setText("/");
    			break;
    		}
    		

        	btn.addActionListener(new ActionListener(){

				@Override
				public void actionPerformed(ActionEvent e) {
					inputView.appendText(String.valueOf(btn.getText()));
				}
        		
        	});
        	
        	panel.add(btn,gbc);
    	}
    	return panel;
		
	}

	public static void main(String args[])
    {
      	try {
    			UIManager.setLookAndFeel(UIManager.getSystemLookAndFeelClassName());
    		} catch (ClassNotFoundException | InstantiationException
    				| IllegalAccessException | UnsupportedLookAndFeelException e) {
    			e.printStackTrace();
    		}
    	java.awt.EventQueue.invokeLater(new Runnable(){
    		@Override
    		public void run() {
    			new Calculator().setVisible(true); //initializes and displays the JFrame
				}
    	});
    }
    

    private void initComponents()
    {
        //settings to initialize a JFrame-------------------->
    	setTitle("Calculator");
    	setDefaultCloseOperation(javax.swing.WindowConstants.EXIT_ON_CLOSE);
        pack();//arranges ui according to window width
        setSize(300,400);//set the size of JFrame, this should be always called after pack().
        //initializing the main UI present inside JFrame
        rootPanel = new JPanel(new BorderLayout());
        keyPanel = new JPanel(new BorderLayout());
        JPanel numPanel = new JPanel(new GridBagLayout());
        GridBagConstraints gbc = new GridBagConstraints();
        //dynamic initialization of buttons
        for(int i=0;i<12;i++)
        {
        	//consider a matrix
        	/* 
        	 * 		x1	x2	x3		
        	 * |	0	1	2	|
        	 * |	3	4	5	|
        	 * |	6	7	8	|
        	 * 
        	 */
        	
        	
        	gbc.gridx = i%3;
        	gbc.gridy = i/3;
        	
        	gbc.weightx = 0.5;
        	gbc.weighty = 0.5;
        	gbc.fill = GridBagConstraints.BOTH; //this ensures, the buttons are spreaded uniformly
        	
        	final JButton btn = new JButton(); //initializing button
        	
        	btn.setText(String.valueOf(i+1)); //set the visible text of button to i+1
        	switch(i) //this switch case is used to change the text for the last three special buttons
        	{
        		case 9:
        			btn.setText("0");
        			break;
        		case 10:
        			btn.setText(".");
        			break;
        		case 11:
        			btn.setBackground(Color.BLUE);
        			btn.setText("=");
        			btn.setForeground(Color.WHITE);
        			break;
        	}

        	btn.addActionListener(new ActionListener(){ //button click is handled here

				@Override
				public void actionPerformed(ActionEvent e) {
					if(btn.getText()!="clr")
						inputView.appendText(String.valueOf(btn.getText()));
					else inputView.setText("");
				}
        		
        	});

        	numPanel.add(btn,gbc);
        	keyPanel.add(numPanel,BorderLayout.CENTER);
        }
        
        rootPanel.add(keyPanel, BorderLayout.CENTER);
        rootPanel.setBackground(Color.BLACK);
        //------------------------WINDOW_SETTING_ENDS-------->

        //display panel contains the inputbox and resultView
        JPanel displayPanel = new JPanel(new BorderLayout());
        
        
        //textField which acts as a display
        inputView = new AJTextField();
        //TODO code to evaluate the text while the user is typing
        displayPanel.add(inputView, BorderLayout.CENTER);
        
        resultView = new MJTextPane();
        displayPanel.add(resultView, BorderLayout.SOUTH);
        
        inputView.setResultTarget(resultView, this);
        
        
        rootPanel.add(displayPanel, BorderLayout.NORTH); //add txtFieled to  our rootPanel
        add(rootPanel); //add rootPanel to JFrame
    }
}
