package com.thing.echo.adapter

import android.content.Context
import android.graphics.Color
import android.support.v7.widget.RecyclerView
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.RelativeLayout
import android.widget.TextView
import com.thing.echo.R
import com.thing.echo.model.Chat
import kotlinx.android.synthetic.main.chat_me.view.*
import android.support.v4.view.GravityCompat
import android.R.attr.gravity
import android.widget.FrameLayout
import android.widget.LinearLayout
import android.R.attr.button




class ChatAdapter(var context: Context, var chats: ArrayList<Chat>) :
    RecyclerView.Adapter<ChatAdapter.ChatViewHolder>() {
    override fun onCreateViewHolder(p0: ViewGroup, p1: Int): ChatViewHolder {
        var chat = chats.get(p1);
        var view = LayoutInflater.from(context)
            .inflate(R.layout.chat_me, p0, false)

        var viewHolder = ChatViewHolder(view)
        viewHolder.setIsRecyclable(false)
        return viewHolder
    }

    override fun getItemCount(): Int {
        return chats.size
    }

    override fun onBindViewHolder(p0: ChatViewHolder, p1: Int) {
        var chat = chats.get(p1);
        p0.chat.text = chat.chat
        //TODO delegate to getItemViewType
        val params = p0.itemView.chatHolder.getLayoutParams() as RelativeLayout.LayoutParams
        if(chat.sendId == "me"){
            //change
            params.addRule(RelativeLayout.ALIGN_PARENT_RIGHT)
            params.marginStart = context.resources.getDimension(R.dimen.padding_medium).toInt()
        } else {
            p0.chatHolder.background = context.resources.getDrawable(R.drawable.right_rounded_rect)
            params.marginEnd = context.resources.getDimension(R.dimen.padding_medium).toInt()
        }
        p0.chatHolder.setLayoutParams(params)
    }

    class ChatViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
        var chat: TextView = itemView.chat
        var chatHolder = itemView.chatHolder
    }
}