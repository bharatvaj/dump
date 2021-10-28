package com.thingstudio.balanceprototype

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.view.View
import androidx.recyclerview.widget.LinearLayoutManager
import com.thingstudio.balanceprototype.adapter.TransactionAdapter
import com.thingstudio.balanceprototype.model.CommodityType
import com.thingstudio.balanceprototype.model.Transaction
import com.thingstudio.balanceprototype.model.TransactionType
import kotlinx.android.synthetic.main.activity_main.*
import java.math.BigDecimal
import java.time.LocalDateTime
import java.util.*
import kotlin.collections.ArrayList

class MainActivity : AppCompatActivity(), View.OnClickListener {

    val transactions =  ArrayList<Transaction>()
    var transactionAdapter : TransactionAdapter? = null

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        transactionAdapter = TransactionAdapter(this, transactions)
        transactionsRecyclerView.adapter = transactionAdapter
        transactionsRecyclerView.layoutManager = LinearLayoutManager(this, LinearLayoutManager.VERTICAL, false)

        button.setOnClickListener(this)
    }

    override fun onClick(v: View?) {
        if(v == null)return
        when(v.id){
            button.id -> {
                val transaction = Transaction("adlfk32434",
                    "Transaction Name",
                    Date(),
                    TransactionType.EXPENSE,
                    "shopping",
                    CommodityType.GOLD,
                    234.34,
                    "sup")
                transactions.add(transaction)
                transactionAdapter?.notifyItemInserted(transactions.indexOf(transaction))
            }
        }
    }


}