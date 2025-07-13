<template>
    <div v-show="menuActiveIndex === 'avatarGallery'" class="x-container">
        <div style="padding: 10px">
            <el-input
                v-model="filter"
                clearable
                placeholder="Filter tags"
                size="mini"
                style="width: 200px; margin-bottom: 10px"
            />
            <div class="gallery">
                <div
                    v-for="avatar in filteredAvatars"
                    :key="avatar.id"
                    class="avatar-card"
                    @click="showAvatarDialog(avatar.id)"
                >
                    <img v-lazy="avatar.thumbnailImageUrl" :alt="avatar.name" class="avatar" />
                    <div class="avatar-name">{{ avatar.name }}</div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    name: 'AvatarGallery',
    inject: ['showAvatarDialog'],
    props: {
        menuActiveIndex: String,
        favoriteAvatars: Array,
        localAvatarFavoriteGroups: Array,
        localAvatarFavorites: Object
    },
    data() {
        return { filter: '' };
    },
    computed: {
        allAvatars() {
            const avatars = [];
            const idSet = new Set();
            for (const fav of this.favoriteAvatars) {
                if (fav.ref && !idSet.has(fav.id)) {
                    const tags = [];
                    if (Array.isArray(fav.ref.tags)) tags.push(...fav.ref.tags);
                    if (fav.groupKey) {
                        const parts = fav.groupKey.split(':');
                        if (parts.length > 1) tags.push(parts[1]);
                    }
                    avatars.push({ ...fav.ref, tags });
                    idSet.add(fav.id);
                }
            }
            for (const group of this.localAvatarFavoriteGroups) {
                const list = this.localAvatarFavorites[group] || [];
                for (const av of list) {
                    if (!idSet.has(av.id)) {
                        avatars.push({ ...av, tags: av.tags || [] });
                        idSet.add(av.id);
                    }
                }
            }
            return avatars;
        },
        filteredAvatars() {
            const f = this.filter.trim().toLowerCase();
            if (!f) return this.allAvatars;
            return this.allAvatars.filter((a) =>
                (a.tags || []).some((t) => String(t).toLowerCase().includes(f))
            );
        }
    }
};
</script>

<style scoped>
.gallery {
    display: flex;
    flex-wrap: wrap;
    gap: 15px;
}
.avatar-card {
    background: #1e1e1e;
    border-radius: 12px;
    width: 200px;
    padding: 5px;
    box-sizing: border-box;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.3);
    text-align: center;
    color: white;
}
.avatar {
    width: 190px;
    height: 190px;
    object-fit: cover;
    border-radius: 8px;
    background-color: #1e1e1e;
    display: block;
    margin: 0 auto;
}
.avatar-name {
    margin-top: 12px;
    font-size: 16px;
    font-weight: bold;
}
</style>
