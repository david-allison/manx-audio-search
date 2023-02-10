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
    return (
        <div>
            <h1>Manx Audio Corpus</h1>
            <h2>Improve your speech with <strong>over {filesToNearest10(statistics.numberOfFiles)}</strong> Manx audio files</h2>
             Search a word in Manx or English
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
                }} onInput={onSearchTextChange} />
                <button style={{marginLeft: 10}} onClick={onSearch}>Search</button>
            </div>
            <div>
                {!results && <>Or practice your Manx with <a href={"/random"}>10 random phrases</a></>}
            </div>
            { results && results.results && results.results.map(x => 
                <>
                <h3>{x.word} — {x.translations.join("; ")}</h3>
                    <div style={{paddingLeft: 20}}>
                    {
                        x.files.map(file => <AudioContainer file={file}/>)
                    }
                    </div>
                </>)}
        </div>
    );
}

const AudioContainer = (props) => {
    const ref = useRef()
    
    const onClick = () => {
        ref.current.play()
    }
    return <>
        <strong onClick={onClick} style={{cursor: "pointer"}}>{props.file.transcription}</strong>
        <AudioPlayer fileName={props.file.fileName} ref={ref}/>
        <div><em>Unknown Gender</em></div>
        <div><em>L2 Manx Speaker</em></div>
        <div/>
    </>
}