package com.cz.rideshare.adapter;

import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import com.bumptech.glide.Glide;
import com.cz.rideshare.R;
import com.cz.rideshare.model.Verification;

import java.util.ArrayList;

/**
 * Created by Home on 31-12-2017.
 */

public class VerificationListAdapter extends RecyclerView.Adapter<VerificationListAdapter.PermissionViewHolder> {

    ArrayList<Verification> verifications = null;

    public VerificationListAdapter(ArrayList<Verification> verifications) {
        this.verifications = verifications;
    }

    @Override
    public PermissionViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View permissionView = LayoutInflater.from(parent.getContext()).inflate(R.layout.verification_item, parent, false);
        return new PermissionViewHolder(permissionView);
    }

    @Override
    public void onBindViewHolder(PermissionViewHolder holder, int position) {
        Verification verification = verifications.get(position);

        if(verification == null)return; //TODO notify the view
                Glide.with(holder.rootView)
                    .load(verification.getIsVerified()? R.drawable.verified : R.drawable.not_verified)
                    .into(holder.detailedPermissionImageView);

        if(verification.getVerificationName() != null){
            holder.detailedPermissionTextView.setText(verification.getVerificationName());
        }
    }

    @Override
    public int getItemCount() {
        return verifications.size();
    }

    class PermissionViewHolder extends RecyclerView.ViewHolder {
        ImageView detailedPermissionImageView = null;
        TextView detailedPermissionTextView = null;
        View rootView = null;
        public PermissionViewHolder(View itemView) {
            super(itemView);
            rootView = itemView;
            detailedPermissionImageView = itemView.findViewById(R.id.detailedVerificationImage);
            detailedPermissionTextView = itemView.findViewById(R.id.detailedVerifcationName);
        }
    }
}
