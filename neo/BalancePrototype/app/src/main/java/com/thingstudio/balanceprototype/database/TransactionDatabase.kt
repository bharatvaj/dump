package com.thingstudio.balanceprototype.database

import androidx.room.Database
import androidx.room.RoomDatabase
import androidx.room.TypeConverters
import com.thingstudio.balanceprototype.converter.CommodityTypeConverter
import com.thingstudio.balanceprototype.converter.DateConverter
import com.thingstudio.balanceprototype.converter.TransactionTypeConverter
import com.thingstudio.balanceprototype.dao.TransactionDao
import com.thingstudio.balanceprototype.model.Transaction

@Database(entities = [Transaction::class], version = 1)
@TypeConverters(TransactionTypeConverter::class, CommodityTypeConverter::class, DateConverter::class)
abstract class TransactionDatabase : RoomDatabase() {

    abstract fun transactionDao(): TransactionDao
}