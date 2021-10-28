package com.thing.echo

import android.support.v7.app.AppCompatActivity
import android.os.Bundle
import android.os.Handler
import android.os.Looper
import android.support.v7.widget.LinearLayoutManager
import android.view.View
import com.thing.echo.adapter.ChatAdapter
import com.thing.echo.model.Chat
import kotlinx.android.synthetic.main.activity_main.*
import kotlinx.android.synthetic.main.toolbar.*
import kotlinx.android.synthetic.main.writer.*
import kotlin.concurrent.thread

class MainActivity : AppCompatActivity(), View.OnClickListener {

    var chats = ArrayList<Chat>()
    var chatAdapter = ChatAdapter(this, chats)

    override fun onClick(p0: View?) {
        when {
            p0?.id == send_btn.id -> {
                val text = writer_edit_text.text.toString()
                chats.add(Chat("me", text))
                chatAdapter.notifyItemInserted(chats.size)
                chats.add(Chat("oofshot", text + " dummy reply"))
                chatAdapter.notifyItemInserted(chats.size)
                writer_edit_text.text.clear()
            }
        }
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        chat_recycler_view.adapter = chatAdapter

        chat_recycler_view.layoutManager = LinearLayoutManager(this, LinearLayoutManager.VERTICAL, false)

        senderName.text = "oofbot"
        chats.add(Chat("oofbot", "To minimize interface, Echo does not provide attachment keys, but they are fully supported. Use a suitable input such as Gboard"))

        send_btn.setOnClickListener(this)


        // Example of a call to a native method
//        sample_text.text = stringFromJNI()
    }


    /**
     * A native method that is implemented by the 'native-lib' native library,
     * which is packaged with this application.
     */
//    external fun stringFromJNI(): String

    companion object {
//
//        // Used to load the 'native-lib' library on application startup.
//        init {
//            System.loadLibrary("native-lib")
//        }
    }
}
