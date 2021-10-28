package com.cz.rideshare.adapter;

import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import com.bumptech.glide.Glide;
import com.cz.rideshare.R;
import com.cz.rideshare.model.Node;

import java.text.SimpleDateFormat;
import java.util.ArrayList;

/**
 * Created by Home on 31-12-2017.
 */

public class PathListAdapter extends RecyclerView.Adapter<PathListAdapter.NodeViewHolder> {

    ArrayList<Node> nodes = null;

    public PathListAdapter(ArrayList<Node> nodes) {
        this.nodes = nodes;
    }

    @Override
    public NodeViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View nodeView = LayoutInflater.from(parent.getContext()).inflate(R.layout.vertical_path, parent, false);
        return new NodeViewHolder(nodeView);
    }

    @Override
    public void onBindViewHolder(NodeViewHolder holder, int position) {
        Node node = nodes.get(position);
        if (holder.nodeHeader != null)
            holder.nodeHeader.setText(node.getLocationName());
        if (holder.nodeTime != null)
            holder.nodeTime.setText(new SimpleDateFormat("hh:mm").format(node.getTimeDelta()));
        if (holder.nodeImage != null)
            Glide.with(holder.rootView)
                    .load(position == 0 ? R.drawable.node_hollow : R.drawable.node_complete)
                    .into(holder.nodeImage);
    }

    @Override
    public int getItemCount() {
        return nodes.size();
    }

    class NodeViewHolder extends RecyclerView.ViewHolder {
        View rootView = null;
        ImageView nodeImage = null;
        TextView nodeTime = null;
        TextView nodeHeader = null;

        public NodeViewHolder(View itemView) {
            super(itemView);
            rootView = itemView;
            nodeImage = rootView.findViewById(R.id.nodeImage);
            nodeTime = rootView.findViewById(R.id.nodeTime);
            nodeHeader = rootView.findViewById(R.id.nodeHeader);
        }
    }
}
