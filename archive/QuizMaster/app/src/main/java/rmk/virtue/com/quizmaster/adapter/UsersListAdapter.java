package rmk.virtue.com.quizmaster.adapter;


import android.content.Context;
import android.content.Intent;
import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import com.squareup.picasso.Picasso;

import java.util.List;

import butterknife.BindView;
import butterknife.ButterKnife;
import de.hdodenhof.circleimageview.CircleImageView;
import rmk.virtue.com.quizmaster.ProfileActivity;
import rmk.virtue.com.quizmaster.R;
import rmk.virtue.com.quizmaster.model.User;

public class UsersListAdapter extends RecyclerView.Adapter<UsersListAdapter.UsersListViewHolder> {
    private Context context;
    private List<User> users;

    public UsersListAdapter(Context context, List<User> users) {
        this.context = context;
        this.users = users;
    }

    @NonNull
    @Override
    public UsersListViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(context).inflate(R.layout.users_list_item, parent, false);
        return new UsersListViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull UsersListViewHolder holder, int position) {
        User user = users.get(position);
        holder.itemView.setOnClickListener(view -> {
            Intent intent = new Intent(context, ProfileActivity.class);
            intent.putExtra(context.getString(R.string.extra_profile_id), user.getId());
            context.startActivity(intent);
        });
        holder.usersListName.setText(user.getName());
        String userDp = user.getDisplayImage();
        if (user.getDisplayImage().isEmpty()) return;
        Picasso.get().load(userDp).error(R.drawable.default_user).into(holder.usersListImageView);
    }

    @Override
    public int getItemCount() {
        return users.size();
    }


    class UsersListViewHolder extends RecyclerView.ViewHolder {

        @BindView(R.id.usersListImageView)
        CircleImageView usersListImageView;
        @BindView(R.id.usersListName)
        TextView usersListName;

        UsersListViewHolder(View itemView) {
            super(itemView);
            ButterKnife.bind(this, itemView);
        }
    }
}
