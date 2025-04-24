package rmk.virtusa.com.quizmaster.adapter;

import com.google.firebase.firestore.CollectionReference;

import java.util.LinkedList;

public class FirestoreList<T> extends LinkedList<T> {

    public Exception CollectionReferenceNullException;

    public FirestoreList() {
        CollectionReferenceNullException = new Exception();
    }

    public FirestoreList(CollectionReference collectionReference) throws Exception {
        if (collectionReference == null) {
            throw CollectionReferenceNullException;
        }
    }

    @Override
    public boolean remove(Object o) {
        //code to remove
        
        return super.remove(o);
    }

    @Override
    public boolean add(T t) {
        //add to firebase
        return super.add(t);
    }
}
