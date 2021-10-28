package com.example.a2wayrecyclerview.adapter

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.recyclerview.widget.RecyclerView
import com.example.a2wayrecyclerview.R
import kotlinx.android.synthetic.main.song_item.view.*

class SongAdapter(private val context: Context, private val songList: ArrayList<String>, private val playingAlbum: ArrayList<String>?) : RecyclerView.Adapter<SongViewHolder>() {
    override fun getItemCount(): Int {
        if(playingAlbum != null && songList.size > 1){
            return 1
        }
        return songList.size
    }

    override fun onBindViewHolder(holder: SongViewHolder, position: Int) {
        holder.itemView.songTextView.text = if(playingAlbum != null && songList.size > 1){
            songList[0] /// @todo return playing song
        }
        else songList[position]
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): SongViewHolder {
        val view = LayoutInflater.from(context).inflate(R.layout.song_item, parent, false)
        return SongViewHolder(view)
    }
}

class SongViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView)