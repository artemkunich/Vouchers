import * as React from 'react'
import { useSelector, useDispatch } from 'react-redux'
import { DomainMembersListFilter, DomainMembersListFilterData } from './listFilter'
import { DomainMembersListElement } from './listElement'
import { FetchingListItemsInfo } from '../common/fetchingListItemsInfo'
import { useLoadOnScroll } from '../../api/helpers'
import { api } from '../../api/api'
import { DomainAccountDto } from '../../types/dtos'
import { RootState } from '../../store/store'
import { User } from 'oidc-client'
import { DomainAccountsQuery } from '../../types/queries'
import { UpdateDomainAccountCommand, UpdateDomainDetailCommand } from '../../types/commands'

interface DomainMembersListProps {
    openDetail: (account: DomainAccountDto) => void
    handleCancel: () => void
}

export const DomainMembersList = ({openDetail, handleCancel}: DomainMembersListProps) => {
    
    const token = useSelector<RootState, User | undefined>(state => state.user.token)
    const currentAccount = useSelector<RootState, DomainAccountDto | undefined>(state => state.domainAccount.currentAccount)

    const [domainMembersQuery, setDomainMembersQuery] = React.useState<DomainAccountsQuery>();
    const [domainAccounts, setDomainAccounts] = React.useState<DomainAccountDto[]>([]);

    React.useEffect(() => {
        if(currentAccount) {
            setDomainMembersQuery({
                ...domainMembersQuery,
                domainId: currentAccount.domainId
            })
        }
        
    }, [currentAccount])

    const getDomainAccounts = React.useCallback(async (query: DomainAccountsQuery) => token && currentAccount ? await api.getDomainAccounts(token, {...query, domainId: currentAccount.domainId}) : [], [api, token, currentAccount])
    const {isFetching, resetPageIndex} = useLoadOnScroll(domainMembersQuery, getDomainAccounts, setDomainAccounts)

    const putDomainAccount = React.useCallback(async (command: UpdateDomainAccountCommand) => {
        if(!token)
            return

        const commandResult = await api.putDomainAccount(token, command)

        if(commandResult)
            resetPageIndex()
    }, [api, token])

    const handleSearch = React.useCallback((filterData: DomainMembersListFilterData) => {
        if(domainMembersQuery)
            setDomainMembersQuery({...domainMembersQuery, ...filterData})
        else if (currentAccount)
            setDomainMembersQuery({domainId: currentAccount.domainId, ...filterData})

        resetPageIndex()
    }, [])

    const openDomainAccountDetail = React.useCallback((domainAccount: DomainAccountDto) => {
        openDetail(domainAccount)
    }, [])

    return <>              
        <DomainMembersListFilter handleSearch={handleSearch} handleCancel={handleCancel}/>
        {
            currentAccount && domainAccounts.map((domainAccount, i) => <DomainMembersListElement key={domainAccount.id} currentAccount={currentAccount} domainAccount={domainAccount} putDomainAccount={putDomainAccount} openDomainAccountDetail={openDomainAccountDetail}/>)
        }
        <FetchingListItemsInfo isFetching={isFetching}/>
    </>  
}