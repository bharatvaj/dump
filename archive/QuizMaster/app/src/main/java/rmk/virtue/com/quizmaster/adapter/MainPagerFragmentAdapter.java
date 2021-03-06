package rmk.virtue.com.quizmaster.adapter;

import android.content.Context;
import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentManager;
import androidx.fragment.app.FragmentPagerAdapter;

import rmk.virtue.com.quizmaster.R;
import rmk.virtue.com.quizmaster.fragment.AnnouncementFragment;
import rmk.virtue.com.quizmaster.fragment.ChatsFragment;

public class MainPagerFragmentAdapter extends FragmentPagerAdapter{

    private Context mContext;

    public MainPagerFragmentAdapter(Context context, FragmentManager fm) {
        super(fm);
        mContext = context;
    }

    // This determines the fragment for each tab
    @Override
    public Fragment getItem(int position) {
        if (position == 0) {
            return new AnnouncementFragment();
        } else {
            return new ChatsFragment();
        }
    }

    // This determines the number of tabs
    @Override
    public int getCount() {
        return 2;
    }

    // This determines the title for each tab
    @Override
    public CharSequence getPageTitle(int position) {
        // Generate title based on item position
        switch (position) {
            case 0:
                return mContext.getString(R.string.announcement_fragment_title);
            case 1:
                return mContext.getString(R.string.chat_fragment_title);
            default:
                return null;
        }
    }

}