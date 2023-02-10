import {useEffect, useRef, useState} from "react";
import React from "react";

export const setRef = (ref, value) => {
    if (ref == null) {
        return
    }

    if (typeof ref === "function") {
        ref(value)
    } else if (ref) {
        ref.current = value
    }
}

export const AudioPlayer =  React.forwardRef((props, forwardRef) => {
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

    setRef(forwardRef, { play: onClick })
    
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
        {clicked && <audio preload={"metadata"} ref={ref} key={props.fileName}>
            <source src={`/audio/${props.fileName}`}/>
        </audio>}
        <img style={{ cursor: "pointer"}} className={props.cssClass} src={require("../assets/images/volume-high.png")} onClick={onClick}/>
    </>)
})