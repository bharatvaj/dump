cmake_minimum_required(VERSION 3.10.2)

set(CMAKE_CXX_STANDARD 11)

add_library(TrueUI ${CMAKE_CURRENT_SOURCE_DIR}/src/TrueUI.cpp ${CMAKE_CURRENT_SOURCE_DIR}/src/temp.cpp)

target_include_directories(TrueUI PUBLIC 
    $<BUILD_INTERFACE:${CMAKE_CURRENT_SOURCE_DIR}/include>
    $<INSTALL_INTERFACE:include>
)

target_compile_features(TrueUI PUBLIC cxx_range_for)