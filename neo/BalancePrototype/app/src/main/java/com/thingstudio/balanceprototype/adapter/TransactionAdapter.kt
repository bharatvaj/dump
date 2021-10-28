package com.thingstudio.balanceprototype.adapter

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.recyclerview.widget.RecyclerView
import com.thingstudio.balanceprototype.BuildConfig
import com.thingstudio.balanceprototype.BuildConfig.DEBUG
import com.thingstudio.balanceprototype.R
import com.thingstudio.balanceprototype.model.Transaction
import com.thingstudio.balanceprototype.model.TransactionType
import kotlinx.android.synthetic.main.transaction_item.view.*

class TransactionAdapter(var context: Context, var transactions: List<Transaction>) : RecyclerView.Adapter<TransactionAdapter.TransactionViewHolder>() {
    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): TransactionViewHolder =
        TransactionViewHolder(LayoutInflater.from(context).inflate(R.layout.transaction_item, parent, false))

    override fun onBindViewHolder(holder: TransactionViewHolder, position: Int) {
        holder.setIsRecyclable(false) // @todo check the repurcussions of this method
        if (DEBUG && position >= itemCount) {
            error("position is greater than list size, pos = ${position}, size = ${itemCount}" )
        }
        val transaction = transactions[position]
        holder.bindItems(transaction)
    }

    override fun getItemCount(): Int = transactions.size

    class TransactionViewHolder(var view: View) : RecyclerView.ViewHolder(view) {
        fun bindItems(transaction: Transaction){
            view.transactionNameTextView.text = transaction.name
            view.transactionDescriptionTextView.text = transaction.notes
            view.transactionSharesTextView.text = transaction.shares.toString()
            view.transactionTypeTextView.text = if ( transaction.type == TransactionType.EXPENSE ) "EXPENSE" else "INCOME"
        }
    }
}