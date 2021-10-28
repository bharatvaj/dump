cmake_minimum_required(VERSION 3.0)
project(solo)

# 3rd-party libraries
find_package(Threads REQUIRED)
find_package(clog)
find_package(Dot)
add_subdirectory(3rdparty/argtable3)

# solo compiler configuration
file(GLOB_RECURSE solo_SRCS "src/*.cpp" "src/solo/*.cpp" "lib/*.c")
add_executable(solo ${solo_SRCS})
target_include_directories(solo PRIVATE ${CMAKE_SOURCE_DIR}/include)
target_compile_features(solo PRIVATE cxx_range_for)

#configuration
include(default.cmake)
configure_file(soloconfig.h.in ${CMAKE_CURRENT_BINARY_DIR}/soloconfig.h)
target_include_directories(solo PRIVATE ${CMAKE_CURRENT_BINARY_DIR})

# link libraries
target_link_libraries(solo argtable3)
target_link_libraries(solo ${CMAKE_THREAD_LIBS_INIT})
target_link_libraries(solo ${CLOG_LIBS})
target_link_libraries(solo ${DOT_LIBS})
