import { useEffect, useState } from "react";
import PageTitle from "../Components/PageTitle";
import * as GameObjectRepository from "../Repositories/GameObjectRepository";
import * as  PlayerInfoRepository from "../Repositories/PlayerInfoRepository";
import useAuth from "../Contexts/Auth";

interface UnlockableAchievment extends GameObjectRepository.Achivement {
  hasUnlocked: boolean
}

const Progresso: React.FC = () => {
  const { user } = useAuth();

  const [allAchievments, setAllAchievments] = useState<UnlockableAchievment[] | null>(null);

  useEffect(() => {
    if (user) {
      PlayerInfoRepository.queryPlayerInfo({ playerId: user.id, token: user.token }).then(playerInfoData => {
        GameObjectRepository.queryAchievments().then(achievmentsData => {
          if ("unlockedAchievments" in playerInfoData) {
            if ("$values" in achievmentsData) {
              setAllAchievments(achievmentsData.$values.map(v => {
                return { ...v, hasUnlocked: playerInfoData.unlockedAchievments.some(ua => ua.Id == v.Id) }
              }));
            }
          }
        })
      })
    }
  });

  const renderAchievments = (allAchievments ?? []).map(a => {
    return <li>{`${a.Title}, hasUnlocked: ${a.hasUnlocked}`}</li>
  })

  return (
    <>
      <PageTitle Text={"Progresso"} onSearchClick={(v) => console.log(allAchievments)} />
      <ul>{renderAchievments}</ul>
    </>
  );
};

export default Progresso;
