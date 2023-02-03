import {useEffect, useRef, useState} from "react";

export const AudioPlayer = (props) => {
    // loading too many audio files causes issues with Chrome - only load if we play
    // This causes complexity here
    const [clicked, setClicked] = useState(false)
    const [autoPlayed, setAutoPlayed] = useState(false)
    const ref = useRef()
    
    const onClick = () => {
        if (!clicked) {
            setClicked(true)
            console.log(!!ref.current)
            return
        }
        ref.current.play()
    }
    
    // another hack: once clicked is true, we can play
    useEffect(() => {
        if (!clicked || autoPlayed) {
            return
        }
        setAutoPlayed(true) // a refresh shouldn't play 1000 sounds 
        if (ref.current) {
            ref.current.play()
        } else {
            console.warn("problem?")
        }
    }, [clicked])
    
    return (<>
        {clicked && <audio preload={"metadata"} ref={ref}>
            <source src={`/audio/${props.fileName}`}/>
        </audio>}
        <img style={{height: "1em"}} src={require("../assets/images/volume-high.png")} onClick={onClick}/>
    </>)
}