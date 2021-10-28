package com.thingstudio.balanceprototype.converter

import androidx.room.TypeConverter
import com.thingstudio.balanceprototype.model.CommodityType
import com.thingstudio.balanceprototype.model.TransactionType
import java.time.Instant
import java.util.*

class DateConverter {
    @TypeConverter
    fun toDate(value: Long) : Date = Date(value)

    @TypeConverter
    fun fromDate(date: Date) = date.time
}