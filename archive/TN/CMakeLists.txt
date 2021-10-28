cmake_minimum_required(VERSION 3.0)
project(TN VERSION 0.2)
find_package(Threads)
find_package(FLTK REQUIRED)
find_package(OpenCV REQUIRED )

add_executable(TN ${CMAKE_CURRENT_SOURCE_DIR}/src/main.cpp)

target_include_directories(TN PRIVATE ${FLTK_INCLUDE_DIRS})
target_include_directories(TN PRIVATE ${CMAKE_CURRENT_SOURCE_DIR}/src)
target_include_directories(TN PRIVATE ${Tesseract_INCLUDE_DIRS})

target_link_libraries(TN ${CMAKE_THREAD_LIBS_INIT} )
target_link_libraries(TN clog)
target_link_libraries(TN ${FLTK_LIBRARIES})
target_link_libraries(TN ${OpenCV_LIBS})
target_link_libraries(TN ${Tesseract_LIBRARIES} )
