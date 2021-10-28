use serde_json::{*};
use std::fs;

/**
 *     "connect": "1",
    "disconnect": "2",
    "help": "3"
 */

fn main() {
    // println!("Hello, world!");

    let filename = "map.json";


    let mut contents = fs::read_to_string(filename)
        .expect("Something went wrong reading the file");
        
    // let payload: Payload = serde_json::from_str(contents.as_mut()).expect("lol");
    // println!("{}", contents);

    
    let json: serde_json::Value =
        serde_json::from_str(contents.as_mut()).expect("JSON was not well-formatted");

    
}
