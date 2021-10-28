package com.example.a2wayrecyclerview

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import androidx.recyclerview.widget.LinearLayoutManager
import com.example.a2wayrecyclerview.adapter.AlbumAdapter
import com.example.a2wayrecyclerview.adapter.SongAdapter
import kotlinx.android.synthetic.main.activity_main.*

class MainActivity : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        val albumList = ArrayList<ArrayList<String>>()

        var songList = ArrayList<String>()
        for(i in 1..100){
            songList.add(i.toString())
        }

        for(i in 1..10){
            albumList.add(songList)
        }

        val albumAdapter = AlbumAdapter(this, albumList)
        recyclerView.adapter = albumAdapter
        recyclerView.layoutManager = LinearLayoutManager(this, LinearLayoutManager.VERTICAL, false)
        albumAdapter.playingAlbum = albumList[0] /// @todo possible crash site

    }
}
