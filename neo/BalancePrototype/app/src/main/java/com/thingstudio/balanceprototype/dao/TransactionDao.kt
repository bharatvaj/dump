package com.thingstudio.balanceprototype.dao

import androidx.room.*
import androidx.sqlite.db.SupportSQLiteQuery
import com.thingstudio.balanceprototype.model.Transaction

@Dao
interface TransactionDao {
    @Insert
    fun insert(transaction: Transaction)

    @Update
    fun update(transaction: Transaction)

    @Delete
    fun delete(transaction: Transaction)

    @RawQuery
    fun getTransactionsForQuery(query: SupportSQLiteQuery) : List<Transaction>
}