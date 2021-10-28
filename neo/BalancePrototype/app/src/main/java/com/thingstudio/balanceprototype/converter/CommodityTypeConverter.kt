package com.thingstudio.balanceprototype.converter

import androidx.room.TypeConverter
import com.thingstudio.balanceprototype.model.CommodityType
import com.thingstudio.balanceprototype.model.TransactionType

class CommodityTypeConverter {
    @TypeConverter
    fun toCommodityType(value: Int) : CommodityType = enumValues<CommodityType>()[value]

    @TypeConverter
    fun fromCommodityType(commodityType: CommodityType) = commodityType.ordinal
}