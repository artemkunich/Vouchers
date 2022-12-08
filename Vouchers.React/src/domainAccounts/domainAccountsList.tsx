import * as React from 'react'
import { useSelector, useDispatch } from 'react-redux'
import { FetchingListItemsInfo } from '../components/fetchingListItemsInfo/fetchingListItemsInfo'
import { DomainAccountsListFilter, Filter} from './domainAccountsListFilter'
import { DomainAccountsListElement } from './domainAccountsListElement'
import { useLoadOnScroll} from '../api/helpers'
import { DomainAccountsQuery } from '../types/queries'
import { DomainAccountDto } from '../types/dtos'
import { RootState } from '../store/store'
import { User } from 'oidc-client'
import { api } from '../api/api'

interface DomainAccountsListProps {
    selectDomainAccount: (account: DomainAccountDto) => void,
    handleCancel: () => {}
}

export const DomainAccountsList = ({selectDomainAccount, handleCancel}: DomainAccountsListProps) => {

    const token = useSelector<RootState, User | undefined>(state => state.user.token)
    const currentAccount = useSelector<RootState, DomainAccountDto | undefined>(state => state.domainAccount.currentAccount)

    const [domainAccountsQuery, setDomainAccountsQuery] = React.useState<DomainAccountsQuery>()
    React.useEffect(() => {
        if(currentAccount) {
            setDomainAccountsQuery({
                ...domainAccountsQuery,
                domainId: currentAccount.domainId
            })
        }
        
    }, [currentAccount])

    const [domainAccounts, setDomainAccounts] = React.useState<DomainAccountDto[]>([]);

    React.useEffect(() => { 
        setDomainAccounts([])
    }, [currentAccount, token])

    const getDomainAccounts = React.useCallback(async (query: DomainAccountsQuery): Promise<DomainAccountDto[]> => token && currentAccount? await api.getDomainAccounts(token, {...query, domainId: currentAccount?.domainId}) : [], [api, token, currentAccount])
    const {isFetching, resetPageIndex} = useLoadOnScroll<DomainAccountsQuery,DomainAccountDto>(domainAccountsQuery, getDomainAccounts, setDomainAccounts)

    const handleSearch = React.useCallback((filter: Filter) => {
        if(domainAccountsQuery)
            setDomainAccountsQuery({...domainAccountsQuery, ...filter})
        
        resetPageIndex()
    }, [])

    return <>
        <DomainAccountsListFilter handleSearch={handleSearch} handleCancel={handleCancel}/>
        {              
            domainAccounts.map((domainAccount, i) => <DomainAccountsListElement key={domainAccount.id} domainAccount={domainAccount} selectDomainAccount={selectDomainAccount}/>)
        }
        <FetchingListItemsInfo isFetching={isFetching}/>
    </>  
}