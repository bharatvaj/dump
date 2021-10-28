package com.cz.rideshare.adapter;

import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;

import com.bumptech.glide.Glide;
import com.cz.rideshare.R;
import com.cz.rideshare.model.Permission;

import java.util.ArrayList;

/**
 * Created by Home on 31-12-2017.
 */

public class PermissionListAdapter extends RecyclerView.Adapter<PermissionListAdapter.PermissionViewHolder> {

    ArrayList<Permission> permissions = null;

    public PermissionListAdapter(ArrayList<Permission> permissions) {
        this.permissions = permissions;
    }

    @Override
    public PermissionViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View permissionView = LayoutInflater.from(parent.getContext()).inflate(R.layout.permission_item, parent, false);
        return new PermissionViewHolder(permissionView);
    }

    @Override
    public void onBindViewHolder(PermissionViewHolder holder, int position) {
        Permission permission = permissions.get(position);

        if (holder.detailedPermissionImageView != null) {
            Glide.with(holder.rootView)
                    .load(permission.getPermissionImage())
                    .into(holder.detailedPermissionImageView);
        }
    }

    @Override
    public int getItemCount() {
        return permissions.size();
    }

    class PermissionViewHolder extends RecyclerView.ViewHolder {
        ImageView detailedPermissionImageView = null;
        View rootView = null;
        public PermissionViewHolder(View itemView) {
            super(itemView);
            rootView = itemView;
            detailedPermissionImageView = itemView.findViewById(R.id.permissionItem);
        }
    }
}
