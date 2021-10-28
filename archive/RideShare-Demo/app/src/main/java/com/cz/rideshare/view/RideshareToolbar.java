package com.cz.rideshare.view;

import android.app.Activity;
import android.content.Context;
import android.content.ContextWrapper;
import android.content.res.TypedArray;
import android.media.Image;
import android.support.v7.app.AppCompatActivity;
import android.util.AttributeSet;
import android.view.View;
import android.widget.ImageButton;
import android.widget.RelativeLayout;
import android.widget.TextView;

import com.cz.rideshare.R;

import org.w3c.dom.Text;

/**
 * Created by Home on 09-01-2018.
 */

public class RideshareToolbar extends RelativeLayout {

    private ImageButton imgButton = null;
    private TextView textView = null;

    public void setHeaderText(String str){
        textView.setText(str);
    }
    public String getHeaderText(){
        return textView.getText().toString();
    }
    public RideshareToolbar(Context context) {
        super(context);
        init(null, 0);
    }

    public RideshareToolbar(Context context, AttributeSet attrSet) {
        super(context, attrSet);
        init(attrSet, 0);
    }

    public RideshareToolbar(Context context, AttributeSet attrSet, int style) {
        super(context, attrSet, style);
        init(attrSet, style);
    }
    private AppCompatActivity scanForActivity(Context cont) {
        if (cont == null)
            return null;
        else if (cont instanceof Activity)
            return (AppCompatActivity) cont;
        else if (cont instanceof ContextWrapper)
            return scanForActivity(((ContextWrapper)cont).getBaseContext());
        return null;
    }
    public void init(AttributeSet attributeSet, int defStyle){
        inflate(getContext(), R.layout.rideshare_toolbar, this);

        imgButton = findViewById(R.id.headerBackButton);
        textView = findViewById(R.id.headerText);
        imgButton.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {
                AppCompatActivity host = scanForActivity(v.getContext());
                host.finish();
            }
        });
        if(attributeSet != null){
            TypedArray a = getContext().obtainStyledAttributes(attributeSet, R.styleable.RideshareToolbar);
            textView.setText(a.getString(R.styleable.RideshareToolbar_headerText));
            a.recycle();
        }
    }
}
