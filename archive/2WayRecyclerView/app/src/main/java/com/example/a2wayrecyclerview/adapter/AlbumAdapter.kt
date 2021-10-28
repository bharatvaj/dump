package com.example.a2wayrecyclerview.adapter

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.LinearSnapHelper
import androidx.recyclerview.widget.RecyclerView
import com.example.a2wayrecyclerview.R
import kotlinx.android.synthetic.main.album_item.view.*

class AlbumAdapter(private val context: Context, private val albumList: ArrayList<ArrayList<String>>) : RecyclerView.Adapter<AlbumViewHolder>() {
    private var _playingAlbum: ArrayList<String>? = null
    var playingAlbum: ArrayList<String>?
        get() = _playingAlbum
        set(value){
            _playingAlbum = value
        }


    override fun getItemCount(): Int {
        if(playingAlbum != null){
            /// @todo return the playing album
            return 1
        }
        return albumList.size
    }

    override fun onBindViewHolder(holder: AlbumViewHolder, position: Int) {
        holder.setIsRecyclable(false)
        val songArrayList = if(playingAlbum != null && playingAlbum!!.size > 1){
             albumList[0] /// @todo should return the playing song
        } else albumList[position]
        val songAdapter = SongAdapter(context, songArrayList, playingAlbum)
        holder.itemView.songRecyclerView.adapter = songAdapter
        holder.itemView.songRecyclerView.layoutManager = LinearLayoutManager(context, LinearLayoutManager.HORIZONTAL, false)
        LinearSnapHelper().attachToRecyclerView(holder.itemView.songRecyclerView)
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): AlbumViewHolder {
        val view = LayoutInflater.from(context).inflate(R.layout.album_item, parent, false)
        return AlbumViewHolder(view)
    }
}

class AlbumViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView)