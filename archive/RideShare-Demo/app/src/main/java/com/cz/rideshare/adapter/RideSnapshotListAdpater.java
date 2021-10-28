package com.cz.rideshare.adapter;

import android.content.Context;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import com.bumptech.glide.Glide;
import com.bumptech.glide.request.RequestOptions;
import com.cz.rideshare.R;
import com.cz.rideshare.listener.RecyclerViewItemClickListener;
import com.cz.rideshare.model.RideSnapshot;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Currency;
import java.util.Locale;

import de.hdodenhof.circleimageview.CircleImageView;


/**
 * Created by Home on 21-12-2017.
 */

public class RideSnapshotListAdpater extends RecyclerView.Adapter<RideSnapshotListAdpater.RideSnapshotViewHolder> {

    private ArrayList<RideSnapshot> rideSnapshots = null;
    private Context context = null;
    private RecyclerViewItemClickListener clickListener = null;

    public RideSnapshotListAdpater(Context context, ArrayList<RideSnapshot> rideSnapshots, RecyclerViewItemClickListener clickListener) {
        this.context = context;
        this.rideSnapshots = rideSnapshots;
        this.clickListener = clickListener;
    }

    @Override
    public RideSnapshotViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View itemView = LayoutInflater.from(parent.getContext()).inflate(R.layout.ride_snapshot, parent, false);
        return new RideSnapshotViewHolder(itemView);
    }

    @Override
    public void onBindViewHolder(RideSnapshotViewHolder holder, int position) {
        RideSnapshot rideSnapshot = rideSnapshots.get(position);
        RequestOptions requestOptions = new RequestOptions();
        requestOptions.centerCrop();

        if(rideSnapshot == null)return;
        if(rideSnapshot.getVehicle() == null)return;

        if (holder.rideImage != null)
            Glide.with(holder.rootView)
                    .load(rideSnapshot.getVehicle().getVehicleType().getTypeImage())
                    .apply(requestOptions)
                    .into(holder.rideImage);
        if (holder.rideType != null)
            holder.rideType.setText(rideSnapshot.getVehicle().getVehicleName());
        if (holder.date != null) {
            SimpleDateFormat sdf = new SimpleDateFormat("EE, MMM dd");
            holder.date.setText(sdf.format(rideSnapshot.getTimeStarted()));
        }
        if (holder.price != null) {
            Locale uk = new Locale("en", "US");
            Currency pound = Currency.getInstance("USD");
            holder.price.setText(pound.getSymbol(uk) + String.valueOf(rideSnapshot.getPrice()));
        }

        if (holder.driverImage != null)
            Glide.with(holder.rootView)
                    .load(rideSnapshot.getDriver().getDisplayPicture())
                    .apply(requestOptions)
                    .into(holder.driverImage);
        if (holder.driverName != null)
            holder.driverName.setText(rideSnapshot.getDriver().getName());
        if (holder.driverRating != null)
            holder.driverRating.setText((int) Math.abs(rideSnapshot.getRating().getRating()) + " ratings");//FIXME

        if (holder.startTime != null)
            holder.startTime.setText((new SimpleDateFormat("HH:mm")).format(rideSnapshot.getTimeStarted()));
        if (holder.endTime != null)
            holder.endTime.setText((new SimpleDateFormat("HH:mm")).format(rideSnapshot.getEnd().getTimeDelta()));
        if (holder.startAddr != null)
            holder.startAddr.setText(rideSnapshot.getStart().getLocationName());
        if (holder.endAddr != null) holder.endAddr.setText(rideSnapshot.getEnd().getLocationName());

    }

    @Override
    public int getItemCount() {
        return rideSnapshots.size();
    }

    class RideSnapshotViewHolder extends RecyclerView.ViewHolder {
        View rootView = null;
        ImageView rideImage = null;
        TextView rideType = null;
        TextView date = null;
        TextView price = null;

        CircleImageView driverImage = null;
        TextView driverName = null;
        TextView driverRating = null;

        TextView startTime = null;
        TextView endTime = null;
        TextView startAddr = null;
        TextView endAddr = null;

        public RideSnapshotViewHolder(View itemView) {
            super(itemView);
            this.rootView = itemView;
            rootView.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    if (clickListener != null)
                        clickListener.onCLick(v, getAdapterPosition());
                }
            });
            rideImage = itemView.findViewById(R.id.snapshotRideImage);
            rideType = itemView.findViewById(R.id.snapshotRideType);
            date = itemView.findViewById(R.id.snapshotDate);
            price = itemView.findViewById(R.id.snapshotPrice);

            driverImage = itemView.findViewById(R.id.snapshotDriverImage);
            driverName = itemView.findViewById(R.id.snapshotDriverName);
            driverRating = itemView.findViewById(R.id.snapshotDriverRating);

            startTime = itemView.findViewById(R.id.snapshotStartTime);
            endTime = itemView.findViewById(R.id.snapshotEndTime);
            startAddr = itemView.findViewById(R.id.snapshotStartAddr);
            endAddr = itemView.findViewById(R.id.snapshotEndAddr);
        }
    }
}