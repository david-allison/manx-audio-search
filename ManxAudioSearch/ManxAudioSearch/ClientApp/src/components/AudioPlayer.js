import {useRef} from "react";

export const AudioPlayer = (props) => {
    
    const ref = useRef()
    const onClick = () => {
        console.log("onClick")
        ref.current.play()
    }
    return (<>
        <audio preload={"metadata"} ref={ref}>
            <source src={`/audio/${props.fileName}`}/>
        </audio>
        <img style={{height: "1em"}} src={require("../assets/images/volume-high.png")} onClick={onClick}/>
    </>)
}