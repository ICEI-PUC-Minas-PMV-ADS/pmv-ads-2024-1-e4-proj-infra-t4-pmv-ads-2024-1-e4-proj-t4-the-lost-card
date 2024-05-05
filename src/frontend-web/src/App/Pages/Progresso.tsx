import { useEffect, useState } from "react";
import PageTitle from "../Components/PageTitle";
import * as GameObjectRepository from "../Repositories/GameObjectRepository";
import * as PlayerInfoRepository from "../Repositories/PlayerInfoRepository";
import useAuth from "../Contexts/Auth";
import AchievmentsCard from "../Components/AchievmentsCard";

interface UnlockableAchievment extends GameObjectRepository.Achivement {
  hasUnlocked: boolean;
}

const Progresso: React.FC = () => {
  const { user } = useAuth();

  const [allAchievments, setAllAchievments] = useState<UnlockableAchievment[]>(
    []
  );

  useEffect(() => {
    PlayerInfoRepository.queryPlayerInfo({
      playerId: user!.id,
    }).then((playerInfoData) => {
      GameObjectRepository.queryAchievments().then((achievmentsData) => {
        if ("unlockedAchievments" in playerInfoData) {
          if ("$values" in achievmentsData) {
            setAllAchievments(
              achievmentsData.$values.map((v) => {
                return {
                  ...v,
                  hasUnlocked: playerInfoData.unlockedAchievments.some(
                    (ua) => ua.Id == v.Id
                  ),
                };
              })
            );
          }
        }
      });
    });
  });

  return (
    <div style={{ width: "100%", gap: "30px" }}>
      <PageTitle
        Text={"Progresso"}
        onSearchClick={(v) => console.log(allAchievments)}
      />
      <div
        style={{
          display: "flex",
          alignContent: "center",
          gap: "30px",
          justifyContent: "center",
          flexDirection: "column",
          padding: "30px 50px",
        }}
      >
        {allAchievments.map((aa) => {
          return (
            <AchievmentsCard
              hasUnlocked={aa.hasUnlocked}
              Description={aa.Description}
              Title={aa.Title}
            />
          );
        })}
      </div>
    </div>
  );
};

export default Progresso;
