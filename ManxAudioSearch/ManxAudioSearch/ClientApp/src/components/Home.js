import React, {useEffect, useState} from 'react';
import "./Home.css"
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
    
    if (loading || statistics == null) {
        return <></>
    }
    
    const filesToNearest10 = n => (Math.floor((n- 1) / 10) ) * 10
    return (
        <div>
            <h1>Manx Audio Corpus</h1>
            <h2>Improve your pronunciation with <strong>over {filesToNearest10(statistics.numberOfFiles)}</strong> Manx audio files</h2>
             Search a word in Manx or English
            <input type={"search"}/>
            <div>
                Or practice your Manx with <a href={"/random"}>10 random phrases</a>
            </div>
        </div>
    );
}