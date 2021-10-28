package com.thing.echo

object EchoHelper {
    fun getUsers(): ArrayList<String> {
        var users = ArrayList<String>()
        users.add("oofbot")
        users.add("me")
        return users
    }
}