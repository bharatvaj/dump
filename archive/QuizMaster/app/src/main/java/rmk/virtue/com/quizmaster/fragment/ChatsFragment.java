package rmk.virtue.com.quizmaster.fragment;


import android.os.Bundle;
import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Toast;

import org.greenrobot.eventbus.Subscribe;
import org.greenrobot.eventbus.ThreadMode;

import java.util.HashMap;
import java.util.Map;

import butterknife.BindView;
import butterknife.ButterKnife;
import butterknife.Unbinder;
import rmk.virtue.com.quizmaster.R;
import rmk.virtue.com.quizmaster.adapter.ChatAdapter;
import rmk.virtue.com.quizmaster.handler.ChatHandler;
import rmk.virtue.com.quizmaster.handler.UserHandler;
import rmk.virtue.com.quizmaster.model.Chat;
import rmk.virtue.com.quizmaster.model.User;

public class ChatsFragment extends BaseFragment {


    @BindView(R.id.chatRecyclerView)
    RecyclerView chatRecyclerView;
    Unbinder unbinder;
    private Map<String, Chat> chats = new HashMap<>();
    ChatAdapter chatAdapter;


    public ChatsFragment() {
        // Required empty public constructor
    }

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_chats, container, false);
        unbinder = ButterKnife.bind(this, view);
        return view;
    }

    @Subscribe(threadMode = ThreadMode.MAIN)
    public void onChat(Chat chat) {
        if (chatAdapter == null) return;
        if (chat.getChatId().isEmpty()) {
            Toast.makeText(getContext(), getString(R.string.chat_fetch_error), Toast.LENGTH_LONG).show();
            return;
        }
        chats.put(chat.getChatId(), chat);
        chatAdapter.notifyDataSetChanged();
    }

    @Subscribe(threadMode = ThreadMode.MAIN)
    public void onUser(User user) {
        ChatHandler.getInstance().getChats(user);
    }

    @Override
    public void onViewCreated(@NonNull View view, @Nullable Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);
        chatAdapter = new ChatAdapter(getContext(), chats);
        chatRecyclerView.setAdapter(chatAdapter);
        chatRecyclerView.setLayoutManager(new LinearLayoutManager(getContext(), LinearLayoutManager.VERTICAL, false));
        UserHandler.getInstance().getUser();
    }

    @Override
    public void onDestroyView() {
        super.onDestroyView();
        unbinder.unbind();
    }
}
