package com.thingstudio.balanceprototype.converter

import androidx.room.TypeConverter
import com.thingstudio.balanceprototype.model.TransactionType

class TransactionTypeConverter {
    @TypeConverter
    fun toTransactionType(value: Int) : TransactionType = enumValues<TransactionType>()[value]

    @TypeConverter
    fun fromTransactionType(transactionType: TransactionType) = transactionType.ordinal
}