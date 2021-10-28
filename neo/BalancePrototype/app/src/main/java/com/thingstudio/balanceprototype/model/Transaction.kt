package com.thingstudio.balanceprototype.model

import androidx.room.Entity
import androidx.room.PrimaryKey
import java.math.BigDecimal
import java.time.LocalDateTime
import java.util.*

enum class TransactionType {
    EXPENSE,
    INCOME
}

enum class CommodityType {
    GOLD,
    INR,
    USD,
    EUR
}

@Entity
data class Transaction(
    @PrimaryKey
    var id: String,
    var name: String,
    var dateTime: Date,
    var type: TransactionType,
    var category: String,
    var commodityType: CommodityType,
    var shares: Double,
    var notes: String) {
}