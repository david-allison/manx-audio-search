import React, {useEffect, useRef, useState} from 'react';
import "./Home.css"
import {AudioPlayer} from "./AudioPlayer";
export const Home = () => {
    
    const [loading, setLoading] = useState(false)
    const [statistics, setStats] = useState(null)
    
    useEffect(() => {
        
        let xx = async () => {
            const response = await fetch('statistics');
            const data = await response.json();
            setStats(data)
            setLoading(false)
        }
        
        
        xx()
        
    }, [])
    
    const [results, setResults] = useState([])
    const [searchInput, setSearchInput] = useState("")
    const [searchType, setSearchType] = useState("gven")

    // TODO: this is lazy
    const onSearch = () => {
        if (searchInput === "") {
            setResults(null)
            return
        }
        const x = async () => {
            const useManx = searchType.includes("gv");
            const useEnglish = searchType.includes("en");
            const response = await fetch(`search/${searchInput}?manx=${useManx}&english=${useEnglish}`);
            const data = await response.json();
            setResults(data)
        }

        x().catch()
    }
    
    const onSearchTextChange = (event) => {
        setSearchInput(event.target.value)
    }
    
    if (loading || statistics == null) {
        return <></>
    }
    
    const selectedLanguageChange = (e) => {
        setSearchType(e.target.value)
    }
    
    const filesToNearest10 = n => (Math.floor((n- 1) / 10) ) * 10
    
    let placeHolder = "Search a word in Manx or English"
    if (searchType === "en") {
        placeHolder = "Search a word in English"
    } else if (searchType === "gv") {
        placeHolder = "Search a word in Manx"
    }
    
    
    return (
        <div>
            <div className={"topContainer"}>
                <div className={"headerContainer"}>
                    <h1>Get speaking with <strong>over {filesToNearest10(statistics.numberOfFiles)}</strong> Manx audio files</h1>
                </div>
                <div className={"searchContainer"}>
                    <div style={{display: "flex"}}>
                        <select name="selectedLanguage" onChange={selectedLanguageChange} style={{marginRight: 10}}>
                            <option value="gven">Manx & English</option>
                            <option value="gv">Manx</option>
                            <option value="en">English</option>
                        </select>
                        <input style={{flex: 1}} type={"search"} onKeyDown={x => {
                            if (x.key === "Enter") {
                                onSearch()
                            }
                        }} onInput={onSearchTextChange} placeholder={placeHolder} />
                        <button style={{marginLeft: 10}} onClick={onSearch}>Search</button>
                    </div>
                </div>
            </div>
                
            <div className={"results"}>
                {/*<div>*/}
                {/*    {(!results || !results.results) && <span className={"randomPhrases"}>Or practice your Manx with <a href={"/random"}>10 random phrases</a></span>}*/}
                {/*</div>*/}
                { results && results.results && results.results.map(x => 
                    <div className={"wordContainer"}>
                        <h3 className={"wordHeader"}>{x.word} — {x.translations.join("; ")}</h3>
                        
                        <div className={"audioContainer"}>
                        {
                            x.files.map(file => <><AudioContainer file={file}/><div className={"wordLine"}/></>)
                        }
                        </div>
                    </div>)
                }
            </div>
        </div>
    );
}

const AudioContainer = (props) => {
    const ref = useRef()
    
    const onClick = () => {
        ref.current.play()
    }
    return <div className={"audio-container-box"}>
        <div style={{flexShrink: 1}}>
            <AudioPlayer cssClass={"home-audio"} fileName={props.file.fileName} ref={ref}/>
        </div>
        <div style={{paddingLeft: 5, paddingTop: 10}}>
            <strong onClick={onClick} style={{cursor: "pointer"}}>{props.file.transcription}</strong>
            <div><em>Pronunciation unverified</em></div>
        </div>
    </div>
}