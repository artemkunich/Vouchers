import { number } from 'prop-types';
import * as React from 'react';

interface UseLoadOnScrollReturnType {
    isFetching: boolean,
    resetPageIndex: (index?: number) => void
}
//(items: TDto[] | ((oldItems: TDto[])=>void))=>void)
export const useLoadOnScroll = <TQuery, TDto>(query: TQuery | undefined, fetch: (q: TQuery)=>Promise<TDto[]>, setItems: React.Dispatch<React.SetStateAction<TDto[]>>) : UseLoadOnScrollReturnType => {

    const [pageIndex, setPageIndex] = React.useState(0)
    const [isFetching, setIsFetching] = React.useState(true);

    const resetPageIndex = React.useCallback((index = 0) => {
        setPageIndex(0)
        setItems([]) 
        setIsFetching(true)
    }, [setItems])

    function handleScroll() {
        
        const mainCollection = document.getElementsByTagName('main');
        if(mainCollection.length < 1)
            return

        const main = mainCollection[0];

        //const container = main.childNodes[0]

        //console.log(main.scrollHeight - main.scrollTop  +  ' ' + main.clientHeight)
        // console.log(window.innerHeight + document.documentElement.scrollTop + ' ' + document.documentElement.offsetHeight);
        
        //if (window.innerHeight + document.documentElement.scrollTop !== document.documentElement.offsetHeight) return;
        if (main.scrollHeight - main.scrollTop - 1 > main.clientHeight) return;

        console.log('Fetch more list items!');
        setIsFetching(true)
    }

    React.useEffect(() => {
        const main = document.getElementsByTagName('main')[0];
        
        main.addEventListener('scroll', handleScroll);
        
        handleScroll()
        
        return () => {
            main.removeEventListener('scroll', handleScroll);
        }
    }, []);

    React.useEffect(() => {
        const effect = async () => {
            if (!isFetching || !query) return

            console.log("Page index " + pageIndex)
            const newItems = await fetch({...query, pageIndex: pageIndex})
            
            if(newItems && newItems.length > 0){
                setItems(oldItems => [...oldItems, ...newItems])
                setPageIndex(i => i + 1)
            }

            setIsFetching(false)

            if(newItems && newItems.length > 0){
                await new Promise(r => setTimeout(r, 100))
                handleScroll()    
            }
        }
           
        effect();
    }, [query, fetch, isFetching, pageIndex]);

    return {isFetching, resetPageIndex};
}