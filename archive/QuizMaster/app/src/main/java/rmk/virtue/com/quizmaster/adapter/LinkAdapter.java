package rmk.virtue.com.quizmaster.adapter;

import android.annotation.SuppressLint;
import android.content.Context;
import android.content.Intent;
import android.net.Uri;
import androidx.annotation.NonNull;
import androidx.appcompat.app.AlertDialog;
import androidx.recyclerview.widget.RecyclerView;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import java.util.Map;

import butterknife.BindView;
import butterknife.ButterKnife;
import rmk.virtue.com.quizmaster.R;
import rmk.virtue.com.quizmaster.handler.FirestoreList;
import rmk.virtue.com.quizmaster.model.Link;

public class LinkAdapter extends RecyclerView.Adapter<LinkAdapter.LinkViewHolder> implements FirestoreList.OnRemoveListener<Link>, FirestoreList.OnModifiedListener<Link> {
    private static final String TAG = "LinkAdapter";
    private Context context;
    private FirestoreList<Link> links;
    private boolean isEditable;

    public LinkAdapter(Context context, FirestoreList<Link> links, boolean isEditable) {
        this.context = context;
        this.links = links;
        this.isEditable = isEditable;
        links.setOnModifiedListener(this);
        links.setOnRemoveListener(this);

    }

    @NonNull
    @Override
    public LinkViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(context).inflate(R.layout.link_item, parent, false);
        return new LinkViewHolder(view);
    }

    private void showAlertDialog(LinkViewHolder holder, Link link) {
        AlertDialog.Builder builder = new AlertDialog.Builder(context);
        builder.setTitle("Edit Url");

        final EditText input = new EditText(context);
        input.setText(link.getUrl());
        builder.setView(input);

        builder.setPositiveButton("OK", (dialog, which) -> {
            String in = input.getText().toString();
            if (in.isEmpty()) {
                Toast.makeText(context, "Empty", Toast.LENGTH_LONG).show();
                return;
            }

            link.setUrl(in);
            links.set(link);
            holder.linkUrl.setText(in);

        });
        builder.setNegativeButton("Remove", (dialog, which) -> {
            links.remove(link);
        });
        builder.show();
    }

    @SuppressLint("ClickableViewAccessibility")
    @Override
    public void onBindViewHolder(@NonNull LinkViewHolder holder, int position) {
        Map.Entry<Link, String> linkPair = links.get(position);
        Link link = linkPair.getKey();
        holder.linkUrl.setText(link.getUrl());
        holder.linkWebsite.setText(LinkFactory.getWebsite(link.getUrl()));
        holder.linkImageView.setImageResource(LinkFactory.getIcon(link.getUrl()));
        holder.itemView.setOnClickListener(view -> {
            Intent intent = new Intent(Intent.ACTION_VIEW, Uri.parse(link.getUrl()));
            context.startActivity(intent);
        });
        if (isEditable) {
            holder.itemView.setOnLongClickListener(view -> {
                showAlertDialog(holder, link);
                return true;
            });
            if (linkPair.getValue().isEmpty()) {
                showAlertDialog(holder, link);
                holder.itemView.performLongClick();
            }
        }
    }

    @Override
    public int getItemCount() {
        return links.size();
    }

    @Override
    public void onRemove(Link link) {
        notifyDataSetChanged();
    }

    @Override
    public void onModified(Link link) {
        notifyDataSetChanged();
    }

    private static class LinkFactory {

        @NonNull
        static String getWebsite(String url) {
            String s = "";
            try {
                s = url.split("www.")[1].split(".com")[0];
            } catch (Exception e) {
                Log.e(TAG, e.getMessage());
            }
            return s;
        }

        static int getIcon(String url) {
            if (url.contains("git")) {
                return R.drawable.github_icon;
            } else if (url.contains("linkedin")) {
                return R.drawable.linkedin_icon;
            } else return R.drawable.web_icon;
        }

    }

    class LinkViewHolder extends RecyclerView.ViewHolder {
        @BindView(R.id.linkImageView)
        ImageView linkImageView;
        @BindView(R.id.linkWebsite)
        TextView linkWebsite;
        @BindView(R.id.linkUrl)
        TextView linkUrl;

        LinkViewHolder(View itemView) {
            super(itemView);
            ButterKnife.bind(this, itemView);
        }
    }
}
