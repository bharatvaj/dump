package rmk.virtue.com.quizmaster;

import androidx.appcompat.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemSelectedListener;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.Spinner;
import android.widget.TextView;

import com.firebase.client.Firebase;
import com.google.gson.Gson;

import org.json.JSONArray;

import rmk.virtue.com.quizmaster.model.TestDetails;


public class AdminActivity extends AppCompatActivity implements OnItemSelectedListener {
    private static final String TAG =AdminActivity.class.getSimpleName() ;

    // private static final String TAG = AdminActivity.class.getSimpleName() ;;


    // DatabaseReference databaseTest;

    Spinner spinner;
    Button submit;
    EditText edit;
    private static final String[]items = {"0","1", "2", "3", "4","5","6","7","8","9","10"};
    Firebase database;
    int testNo;
    View testname;
    View from;
    View to;
    Button next;
    Button delete;
String json;
  //  JsonArray jsonArray;
JSONArray jsonArray = new JSONArray();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        setContentView(R.layout.activity_admin);
        Firebase.setAndroidContext(this);

        database = new Firebase("https://adminnapp.firebaseio.com/");





        spinner = (Spinner)findViewById(R.id.dropdown);
        spinner.setEnabled(true);
        ArrayAdapter<String> adapter = new ArrayAdapter<String>(AdminActivity.this,android.R.layout.simple_spinner_item,items);
        adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        spinner.setAdapter(adapter);
        spinner.setOnItemSelectedListener(this);


        

       
        submit = (Button)findViewById(R.id.button);
        submit.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                LinearLayout ll= (LinearLayout)findViewById(R.id.info);
                submit.setEnabled(false);
                for(int i=1; i<= testNo;i++) {
                    EditText testname = (EditText) ll.findViewById(i+14);
                    String name = testname.getText().toString();
                    EditText from = (EditText) ll.findViewById(i+17);
                    String fromText = from.getText().toString();
                    EditText to = (EditText) ll.findViewById(i+24);
                    String toText = to.getText().toString();

                    Log.v("Admin"," "+name+" "+fromText+" "+toText);


                    TestDetails test= new TestDetails(name,fromText,toText);

                    Gson gson= new Gson();
                    json = gson.toJson(test);
                    Log.d(TAG, "onClick: "+json);



                     jsonArray.put(json);

                     String id = database.push().getKey();
                    Log.d(TAG, "onClick: "+id);
                     database.child(id).setValue(test);



                    }

       /*   String id = databaseTest.push().getKey();
                String mcq = null;
                String start=  null;
                String end = null;
                TestDetails testDetails = new TestDetails(mcq, start, end);
                databaseTest.child(id).setValue(testDetails);
               // Toast.show(this, "value added", Toast.LENGTH_LONG).show();// */


              Log.d(TAG, "JSONARRay: "+ jsonArray.toString());

            }
        });



    }


    @Override
    public void onItemSelected(AdapterView<?> adapterView, View view, final int position, long l) {
        Log.v("AdminAct", "Line 122"+position);
        testNo = position;
        LinearLayout ll= (LinearLayout)findViewById(R.id.info);
        for(int i=1;i<=position;i++)
        {
            TextView tv1 = new TextView(this);
            tv1.setText("Test Name " + i);
            //tv1.setId(i);
            testname = new EditText(this);
            testname.setId(i+14);
            TextView tv2 = new TextView(this);
            tv2.setText("Enter Start time - 24hr format(HH:MM)");
            //tv2.setId(i+2);
            from= new EditText(this);
            from.setId(i+17);
            TextView tv3 = new TextView(this);
            tv3.setText("Enter end time - 24hr format (HH:MM)");
            //tv3.setId(i+4);
            to= new EditText(this);
            to.setId(i+24);





            ll.addView(tv1);
            ll.addView(testname);
            ll.addView(tv2);
            ll.addView(from);
            ll.addView(tv3);
            ll.addView(to);
            spinner.setEnabled(false);
            submit.setEnabled(true);

        }

        //spinner disable
        //button enable and siable it initially

           }


    @Override

    public void onNothingSelected(AdapterView<?> adapterView) {

    }
}
