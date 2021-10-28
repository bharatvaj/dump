package com.thing.bluetoothapp;

import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothSocket;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;

import java.io.DataInputStream;
import java.io.DataOutput;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.UUID;

public class MainActivity extends AppCompatActivity {

    public static String TAG = "BLE";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        new Thread(()-> {
            BluetoothSocket s = null;
            BluetoothDevice device = BluetoothAdapter.getDefaultAdapter().getRemoteDevice("B8:27:EB:A5:A1:70");
            Log.e(TAG, device.getName() + "G");
            try {
                s = device.createInsecureRfcommSocketToServiceRecord(UUID.fromString("0000110c-0000-1000-8000-00805f9b34fbs"));
                s.connect();
                DataInputStream dis = new DataInputStream(s.getInputStream());
                DataOutputStream dos = new DataOutputStream(s.getOutputStream());
                byte ib[] = new byte[300];
                Log.e(TAG, dis.readUTF());
                dos.write("hi".getBytes());
            } catch (IOException e) {
                Log.e(TAG, e.getMessage());
            } finally {
                if(s != null) {
                    try {
                        s.close();
                    } catch (IOException e1) {
                        Log.e(TAG, e1.getMessage());
                    }
            }
        }}).start();

    }

}
