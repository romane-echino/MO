import React from "react";

interface IAppProps{

}

interface IAppState{

}

export default class App extends React.Component<IAppProps,IAppState>{
    constructor(props:IAppProps){
        super(props);
    }

    render(): React.ReactNode {
        return(
            <div>
                Bonjour monde
            </div>
        )
    }
}